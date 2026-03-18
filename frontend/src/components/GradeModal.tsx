import React, { useState, useEffect } from 'react';
import { X, Upload, FileText } from 'lucide-react';
import { Grade } from '../types';

interface GradeModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (courseName: string, description: string, file?: File) => Promise<void>;
  grade?: Grade | null;
}

const GradeModal: React.FC<GradeModalProps> = ({ isOpen, onClose, onSubmit, grade }) => {
  const [courseName, setCourseName] = useState('');
  const [description, setDescription] = useState('');
  const [file, setFile] = useState<File | null>(null);
  const [loading, setLoading] = useState(false);
  const [dragActive, setDragActive] = useState(false);

  useEffect(() => {
    if (grade) {
      setCourseName(grade.courseName);
      setDescription(grade.description);
    } else {
      setCourseName('');
      setDescription('');
      setFile(null);
    }
  }, [grade]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await onSubmit(courseName, description, file || undefined);
      onClose();
      setCourseName('');
      setDescription('');
      setFile(null);
    } finally {
      setLoading(false);
    }
  };

  const handleDrag = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (e.type === 'dragenter' || e.type === 'dragover') {
      setDragActive(true);
    } else if (e.type === 'dragleave') {
      setDragActive(false);
    }
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);
    
    if (e.dataTransfer.files && e.dataTransfer.files[0]) {
      setFile(e.dataTransfer.files[0]);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setFile(e.target.files[0]);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-xl shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <h2 className="text-2xl font-bold text-gray-900">
            {grade ? 'Not Düzenle' : 'Yeni Not Ekle'}
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 transition-colors"
          >
            <X className="w-6 h-6" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-6">
          <div>
            <label htmlFor="courseName" className="block text-sm font-medium text-gray-700 mb-2">
              Ders Adı *
            </label>
            <input
              id="courseName"
              type="text"
              value={courseName}
              onChange={(e) => setCourseName(e.target.value)}
              className="input"
              placeholder="Örn: Matematik, Fizik, Kimya"
              required
              maxLength={200}
            />
          </div>

          <div>
            <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-2">
              Açıklama *
            </label>
            <textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="input min-h-[120px] resize-y"
              placeholder="Not hakkında detaylı açıklama yazın..."
              required
              maxLength={1000}
            />
            <p className="text-xs text-gray-500 mt-1">{description.length}/1000 karakter</p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Dosya Yükle (Opsiyonel)
            </label>
            <div
              onDragEnter={handleDrag}
              onDragLeave={handleDrag}
              onDragOver={handleDrag}
              onDrop={handleDrop}
              className={`border-2 border-dashed rounded-lg p-8 text-center transition-colors ${
                dragActive ? 'border-primary-500 bg-primary-50' : 'border-gray-300 hover:border-gray-400'
              }`}
            >
              <input
                type="file"
                onChange={handleFileChange}
                className="hidden"
                id="file-upload"
                accept=".pdf,.doc,.docx,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp,.webp"
              />
              <label htmlFor="file-upload" className="cursor-pointer">
                <Upload className="w-12 h-12 text-gray-400 mx-auto mb-4" />
                <p className="text-sm text-gray-600 mb-2">
                  Dosyayı sürükleyip bırakın veya <span className="text-primary-600 font-medium">seçin</span>
                </p>
                <p className="text-xs text-gray-500">
                  PDF, Word, Excel, Resim dosyaları desteklenir
                </p>
              </label>

              {file && (
                <div className="mt-4 flex items-center justify-center space-x-2 p-3 bg-gray-50 rounded-lg">
                  <FileText className="w-5 h-5 text-gray-600" />
                  <span className="text-sm text-gray-900">{file.name}</span>
                  <button
                    type="button"
                    onClick={() => setFile(null)}
                    className="text-red-600 hover:text-red-700"
                  >
                    <X className="w-4 h-4" />
                  </button>
                </div>
              )}
            </div>
          </div>

          <div className="flex items-center space-x-3 pt-4">
            <button
              type="submit"
              disabled={loading}
              className="btn btn-primary flex-1 flex items-center justify-center space-x-2"
            >
              {loading ? (
                <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
              ) : (
                <span>{grade ? 'Güncelle' : 'Kaydet'}</span>
              )}
            </button>
            <button
              type="button"
              onClick={onClose}
              className="btn btn-secondary flex-1"
            >
              İptal
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default GradeModal;